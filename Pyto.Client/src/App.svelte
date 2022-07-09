<script lang="ts">
  import { AuthorizationState } from "./AppState";
  import Aside from "./components/Aside.svelte";
  import Authorization from "./components/Authorization.svelte";
  import Navbar from "./components/Navbar.svelte";
  import TodoList from "./components/TodoList.svelte";
  import Tailwind from "./Tailwind.svelte";
</script>

<main class="flex-grow flex items-center bg-nord-0 text-nord-6">
  {#if $AuthorizationState === "unauthorized"}
    <Authorization />
  {:else}
    <Aside />
    <div class="flex flex-grow h-full flex-col items-center">
      <Navbar />
      <TodoList />
    </div>
  {/if}

  <Tailwind />
</main>

<style>
</style>
