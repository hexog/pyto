<script lang="ts">
  import { login, register } from "../lib/AuthenticationService";

  let state: "login" | "register" = "login";

  let inputEmail = "";
  let inputPassword = "";
  let repeatPassword = "";

  const onSubmit = async () => {
    if (state === "login") {
      await login(inputEmail, inputPassword)
    } else {
        await register(inputEmail, inputPassword)
    }
  };
</script>

<div>
  <form on:submit|preventDefault={onSubmit}>
    {#if state === "login"}
      <label for="login-email">
        Email:
        <input type="email" bind:value={inputEmail} id="login-email" />
      </label>
      <label for="login-password"
        >Password:
        <input type="password" bind:value={inputPassword} id="login-password" />
      </label>
    {:else}
      <label for="register-email">
        Email:
        <input type="email" bind:value={inputEmail} id="register-email" />
      </label>
      <label for="register-password"
        >Password:
        <input
          type="password"
          bind:value={inputPassword}
          id="register-password"
        />
      </label>

      <label for="register-repeatpassword"
        >Repeat password:
        <input
          type="password"
          bind:value={repeatPassword}
          id="register-repeatpassword"
        />
      </label>
    {/if}

    <button type="submit">Submit</button>

    <button on:click={() => (state = state === "login" ? "register" : "login")}
      >{state}</button
    >
  </form>
</div>
