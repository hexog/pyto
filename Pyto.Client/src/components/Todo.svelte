<script lang="ts">
  import type { TodoModel, TodoState } from "src/lib/TodoListService";
  import { createEventDispatcher } from "svelte";

  export let id: string;
  export let content: string;
  export let state: TodoState;
  let isChecked = state === 'checked'

  let isEditing = false;

  let input: HTMLInputElement;

  sessionStorage.ge;

  const startEditing = () => {
    isEditing = true;
    setTimeout(() => input.focus(), 0);
  };

  const stopEditing = () => {
    isEditing = false
    todoChanged()
  };

  const onTodoChanged = createEventDispatcher();

  const todoChanged = () => {
    const todo: TodoModel = {
      id,
      content,
      state: isChecked ? 'checked' : 'unchecked',
    };
    
    onTodoChanged("todochange", todo);
  };
</script>

<div class="flex flex-row items-center gap-x-1 my-3 mx-5 w-4/12 h-6">
  <div class="flex items-center justify-center">
    <input
      class="todo-check"
      type="checkbox"
      {id}
      bind:checked={isChecked}
      on:change={() => todoChanged()}
    />
    <label for={id}><span /></label>
  </div>
  <div on:click={startEditing} class="flex flex-grow">
    {#if !isEditing}
      <div class="flex-grow px-2 cursor-pointer">
        {content}
      </div>
    {/if}
    <input
      type="text"
      bind:value={content}
      bind:this={input}
      class="flex flex-grow outline-1 py-1 px-2"
      class:invisible={!isEditing}
      on:blur={stopEditing}
    />
  </div>
</div>

<style lang="postcss">
  .todo-check {
    display: none;
  }

  .todo-check + label span {
    display: block;
    width: 25px;
    height: 19px;
    background-color: rgba(0, 0, 0, 0);
  }

  .todo-check + label span:before,
  .todo-check + label span:after {
    -webkit-transition: all 0.3s ease-in-out;
    -moz-transition: all 0.3s ease-in-out;
    transition: all 0.2s ease-in-out;
    content: "";
    position: absolute;
    z-index: 1;
    width: 1.25rem;
    height: 1.25rem;
    background: transparent;
    border: 2px solid var(--primary);
  }

  .todo-check + label span:after {
    z-index: 0;
    border: none;
  }

  .todo-check:checked + label span:before {
    z-index: 100;
    background-color: var(--primary);
  }

  .todo-item-name {
    display: flex;
    flex-grow: 1;
    justify-content: center;
    align-items: center;
  }

  .todo-item-name * {
    display: flex;
    flex-grow: 1;

    height: 2rem;
    padding: 0 0 0 0.5rem;
  }

  .todo-item-name-text {
    display: flex;
    justify-content: flex-start;
    align-items: center;
  }

  .todo-item-name-edit {
    border: none;
  }

  .invisible {
    display: none;
  }
</style>
