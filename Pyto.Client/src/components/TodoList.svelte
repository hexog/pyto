<script lang="ts">
  import TodoListService from "../lib/TodoListService";
  import Button from "./Form/Button.svelte";
import InputText from "./Form/InputText.svelte";
  import Todo from "./Todo.svelte";
  const todoList = TodoListService.todoList;

  function onTodoChange(todoEvent) {
    TodoListService.updateTodo(todoEvent.detail);
  }

  let newTodoContent = "";
  let isAddingTodo = false;

  function addTodo() {
    isAddingTodo = true;
  }

  function cancelAddTodo() {
    isAddingTodo = false;
  }

  function confirmAddTodo() {
    isAddingTodo = false;
    // TodoListService.addTodo
    newTodoContent = "";
  }
</script>

<div
  class="flex flex-col flex-grow w-full h-full items-center bg-nord-0 text-nord-6 shadow-black"
>
  <div class="flex flex-col flex-grow w-11/12 lg:w-6/12 my-12 gap-y-1">
    {#each $todoList as todo}
      <Todo {...todo} on:todochange={onTodoChange} />
    {/each}

    {#if isAddingTodo}
      <div>
        <!-- <input type="text" id="new-todo-input" bind:value={newTodoContent} /> -->
        <InputText bind:value={newTodoContent} />
      </div>
      <div>
        <Button on:click={confirmAddTodo}>Add</Button>
        <Button on:click={cancelAddTodo}>Cancel</Button>
      </div>
    {:else}
      <div>
        <Button on:click={addTodo}>Add</Button>
      </div>
    {/if}
  </div>
</div>
