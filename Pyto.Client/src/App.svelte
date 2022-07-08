<script lang="ts">
  import { AuthorizationState } from "./AppState";
  import Authorization from "./components/Authorization.svelte";
  import TodoList from "./components/TodoList.svelte";
  import Tailwind from "./Tailwind.svelte";
</script>

<main class="">
  {#if $AuthorizationState === "unauthorized"}
    <Authorization />
  {:else}
    <TodoList />
  {/if}

  <Tailwind />
</main>

<style>
</style>
