<script lang="ts">
  import { onMount } from "svelte";
  import { AuthorizationState } from "./AppState";

  import logo from "./assets/svelte.png";
  import Authorization from "./components/Authorization.svelte";
  import Counter from "./components/Counter.svelte";
  import Todo from "./components/Todo.svelte";
  import TodoList from "./components/TodoList.svelte";
  import { readAccessToken } from "./lib/AuthenticationService";
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
