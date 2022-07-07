import { writable } from "svelte/store";

export const AuthorizationState = writable<"authorized"|"unauthorized">("unauthorized")