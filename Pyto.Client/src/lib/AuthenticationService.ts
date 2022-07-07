import { AuthorizationState } from "../AppState";
import { urlBase } from "./api/Common";

const commonHeaders = {
    "Content-Type": "application/json",
};

export function login(email: string, password: string) {
    return (tokenReadLock = new Promise<void>(async (resolve) => {
        const request = fetch(`${urlBase}/account/login`, {
            method: "POST",
            body: JSON.stringify({ email, password }),
            headers: commonHeaders,
        });

        const result = await handleResponse(request);
        useToken(result.accessToken, result.validTo, result.refreshToken);
        resolve();
    }));
}

export function register(email: string, password: string) {
    return tokenReadLock = new Promise<void>(async (resolve) => {
        const request = fetch(`${urlBase}/account/register`, {
            method: "POST",
            body: JSON.stringify({ email, password }),
            headers: commonHeaders,
        });

        const result = await handleResponse(request);
        useToken(result.accessToken, result.validTo, result.refreshToken);
        resolve();
    });
}

function useToken(accessToken: string, validTo: Date, refreshToken: string) {
    sessionStorage.setItem(
        "accessToken",
        JSON.stringify({ accessToken, validTo })
    );
    localStorage.setItem("refreshToken", JSON.stringify({ refreshToken }));
}

function clearTokens() {
    sessionStorage.removeItem("accessToken");
    localStorage.removeItem("refreshToken");
}

let tokenReadLock: Promise<void> | null = null;

export async function readAccessToken(): Promise<string> {
    if (tokenReadLock) {
        await tokenReadLock;
    }

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

function refreshTokens() {
    const promise = new Promise<{
        accessToken: any;
        validTo: any;
        refreshToken: any;
    }>(async (resolve) => {
        const refreshToken = JSON.parse(localStorage.getItem("refreshToken"));

        if (!refreshToken) {
            throw new Error("Refresh token is not set");
        }

        const request = fetch(
            `${urlBase}/account/refresh?token=${encodeURIComponent(
                refreshToken.refreshToken
            )}`,
            {
                method: "POST",
            }
        );

        const result = await handleResponse(request)
        resolve(result)
    })

    tokenReadLock = promise.then(x => undefined)
    return promise
}

async function handleResponse(fetchPromise: Promise<Response>) {
    const response = await fetchPromise;
    const json = await response.json();

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

    const json = await response.json();

    if (!response.ok) {
        throw new Error(json.message);
    }

    clearTokens();
}
