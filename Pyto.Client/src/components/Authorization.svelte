<script lang="ts">
  import { prevent_default } from "svelte/internal";
  import { login, register } from "../lib/AuthenticationService";
  import Button from "./Form/Button.svelte";
  import InputEmail from "./Form/InputEmail.svelte";
  import InputPassword from "./Form/InputPassword.svelte";
  import InputText from "./Form/InputText.svelte";

  let state: "login" | "register" = "login";

  let inputEmail = "";
  let inputPassword = "";
  let repeatPassword = "";

  let isLoggingIn = false;

  const onSubmit = async () => {
    isLoggingIn = true;
    if (state === "login") {
      await login(inputEmail, inputPassword);
    } else {
      await register(inputEmail, inputPassword);
    }

    isLoggingIn = false;
  };
</script>

<div class="flex flex-grow items-center justify-center">
  <form on:submit|preventDefault={onSubmit} class="flex flex-col">
    {#if state === "login"}
      <label for="login-email" class="input-block">
        <p>Email:</p>
        <InputEmail bind:value={inputEmail} id="login-email" />
      </label>
      <label for="login-password" class="input-block">
        <p>Password:</p>
        <InputPassword bind:value={inputPassword} id="login-password" />
      </label>
    {:else}
      <label for="register-email" class="input-block">
        <p>Email:</p>
        <InputEmail bind:value={inputEmail} id="register-email" />
      </label>
      <label for="register-password" class="input-block">
        <p>Password:</p>
        <InputPassword bind:value={inputPassword} id="register-password" />
      </label>

      <label for="register-repeatpassword" class="input-block">
        <p>Repeat password:</p>
        <InputPassword
          bind:value={repeatPassword}
          id="register-repeatpassword"
        />
      </label>
    {/if}

    <div class="flex justify-end mt-2">
      <Button color="primary" isActive={isLoggingIn}>Submit</Button>
    </div>

    <div class="flex justify-center mt-3 text-sm">
      <Button
        on:click={(e) => {
          e.preventDefault();
          state = state === "login" ? "register" : "login";
        }}
        color="link"
      >
        {state === "login" ? "Register instead" : "Login instead"}
      </Button>
    </div>
  </form>
</div>

<style scoped lang="postcss">
  .input-block {
    @apply my-1.5;
  }

  .input-block p {
    @apply mb-1 ml-1 font-medium;
  }
</style>
