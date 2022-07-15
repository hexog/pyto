import { AuthorizationState } from "../AppState";
import { urlBase } from "./api/Common";

const commonHeaders = {
    "Content-Type": "application/json",
};

const readTokenLockName = 'readTokenLock'
const lockReadToken = <Result>(fn: () => Result | Promise<Result>) => {
    if (navigator.locks) {
        return navigator.locks.request(readTokenLockName, {
            mode: "exclusive",
        }, lock => fn())
    }

    return (async () => await fn())()
}

export function readAccessToken(): Promise<string> {
    function refreshTokens() {
        const refreshToken = JSON.parse(localStorage.getItem("refreshToken"));

        if (!refreshToken) {
            throw new Error("Refresh token was not found");
        }

        const request = fetch(
            `${urlBase}/account/refresh?token=${encodeURIComponent(
                refreshToken.refreshToken
            )}`,
            {
                method: "POST",
            }
        );

        return handleLoginResponse(request)
    }

    async function readAccessToken(): Promise<string> {
        const { accessToken, validTo } = JSON.parse(
            sessionStorage.getItem("accessToken")
        );

        if (Date.parse(new Date().toUTCString()) < Date.parse(validTo)) {
            AuthorizationState.set("authorized");
            return accessToken;
        }

        try {
            const {
                accessToken: newAccessToken,
                validTo: newValidTo,
                refreshToken,
            } = await refreshTokens();
            useToken(newAccessToken, newValidTo, refreshToken);
            return newAccessToken;
        } catch (e) {
            AuthorizationState.set("unauthorized");
            throw e;
        }
    }

    return lockReadToken(readAccessToken)
}

export function login(email: string, password: string) {
    return lockReadToken(async () => {
        const request = fetch(`${urlBase}/account/login`, {
            method: "POST",
            body: JSON.stringify({ email, password }),
            headers: commonHeaders,
        });

        const result = await handleLoginResponse(request);
        useToken(result.accessToken, result.validTo, result.refreshToken);
    }).catch(x => console.log(x))
}

export function register(email: string, password: string) {
    return lockReadToken(async () => {
        const request = fetch(`${urlBase}/account/register`, {
            method: "POST",
            body: JSON.stringify({ email, password }),
            headers: commonHeaders,
        });

        const result = await handleLoginResponse(request);
        useToken(result.accessToken, result.validTo, result.refreshToken);
    });
}

function useToken(accessToken: string, validTo: Date, refreshToken: string) {
    sessionStorage.setItem(
        "accessToken",
        JSON.stringify({ accessToken, validTo })
    );
    localStorage.setItem("refreshToken", JSON.stringify({ refreshToken }));

    AuthorizationState.set('authorized')
}

function clearTokens() {
    sessionStorage.removeItem("accessToken");
    localStorage.removeItem("refreshToken");
}

async function handleLoginResponse(fetchPromise: Promise<Response>) {
    const response = await fetchPromise;
    const json = await response.json().catch(_ => { message: 'Invalid response' });

    if (response.ok) {
        if (!json.accessToken || !json.accessTokenValidTo || !json.refreshToken) {
            throw new Error("Invalid response from the server");
        }

        AuthorizationState.set("authorized");
        return {
            accessToken: json.accessToken,
            validTo: json.accessTokenValidTo,
            refreshToken: json.refreshToken,
        };
    }

    clearTokens();
    throw new Error(json.message);
}

export async function logout(): Promise<never | void> {
    const token = await readAccessToken();
    const response = await fetch(`${urlBase}/account/logout`, {
        method: "POST",
        headers: {
            Authorization: `Bearer ${token}`,
        },
    });


    if (!response.ok) {
        const json = await response.json();
        throw new Error(json.message);
    }

    clearTokens();
}
