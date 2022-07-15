<script lang="ts">
  import TodoListService, { TodoModel } from "../lib/TodoListService";
  import Button from "./Form/Button.svelte";
  import InputText from "./Form/InputText.svelte";
  import InputTextArea from "./Form/InputTextArea.svelte";
  import Todo from "./Todo.svelte";
  const todoList = TodoListService.todoList;

  function onTodoChange(todoEvent) {
    TodoListService.updateTodo(todoEvent.detail);
  }

  let newTodoInput: { focus: () => void };
  let newTodoContent = "";
  let isAddingTodo = false;

  function addTodo() {
    isAddingTodo = true;
    setTimeout(() => newTodoInput.focus());
  }

  function cancelAddTodo() {
    isAddingTodo = false;
  }

  function confirmAddTodo() {
    isAddingTodo = false;

    if (newTodoContent) {
      TodoListService.addTodo({
        content: newTodoContent,
      });
      newTodoContent = "";
    }
  }

  function onTodoDelete(event) {
    const todo = event.detail as TodoModel;
    TodoListService.deleteTodo(todo);
  }
</script>

<div
  class="flex flex-col flex-grow w-full h-full items-center bg-nord-0 text-nord-6 shadow-black"
>
  <div class="flex flex-col flex-grow w-11/12 lg:w-6/12 my-12 gap-y-1">
    {#each $todoList as todo}
      <!-- TODO: drag n drop -->
      <Todo
        {...todo}
        on:todochange={onTodoChange}
        on:tododelete={onTodoDelete}
      />
    {/each}

    {#if isAddingTodo}
      <div class="mb-2">
        <!-- <input type="text" id="new-todo-input" bind:value={newTodoContent} /> -->
        <InputTextArea
          bind:value={newTodoContent}
          bind:this={newTodoInput}
          rows={newTodoContent.split("\n").length}
        />
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
