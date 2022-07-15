<script>
  import { logout } from "../lib/AuthenticationService";
  import Button from "./Form/Button.svelte";
</script>

<header class="h-16 bg-nord-1 text-nord-6 w-full flex flex-row items-center">
  <div class="flex-grow" />
  <div class="mr-3">
    <Button on:click={() => logout()} color="link">Logout</Button>
  </div>
</header>
