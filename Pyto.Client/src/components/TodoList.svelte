<script lang="ts">
  import TodoListService from "../lib/TodoListService";
  import Todo from "./Todo.svelte";
  const todoList = TodoListService.todoList;

  function onTodoChange(todoEvent) {
    TodoListService.updateTodo(todoEvent.detail)
  }
</script>

<div>
  {#each $todoList as todo}
    <Todo {...todo} on:todochange={onTodoChange}/>
  {/each}
  <!-- <Todo /> -->
</div>
