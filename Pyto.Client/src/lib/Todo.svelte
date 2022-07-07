<script lang="ts">
  export let key: number | string = Math.random();
  export let name: string;
  export let isChecked: boolean = false;

  let isEditing = false;

  let input: HTMLInputElement;

  const startEditing = () => {
    isEditing = true;
    setTimeout(() => input.focus(), 0);
  };

  const stopEditing = () => (isEditing = false);
</script>

<div class="todo-item">
  <div class="todo-item-checked">
    <input class="todo-item-cheked-input" type="checkbox" id={key.toString()} checked={isChecked} />
    <label for={key.toString()}><span /></label>
  </div>
  <div on:click={startEditing} class="todo-item-name">
    {#if !isEditing}
      <div class="todo-item-name-text">
        {name}
      </div>
    {/if}
    <input
      type="text"
      bind:value={name}
      bind:this={input}
      class="todo-item-name-edit"
      class:invisible={!isEditing}
      on:blur={stopEditing}
    />
  </div>
</div>

<style>
  .todo-item-checked {
      margin: 0 0.75rem 0 0.25rem;
  }

  .todo-item-cheked-input {
    display: none;
  }

  .todo-item-cheked-input + label span {
    display: inline-block;
    width: 25px;
    height: 19px;
    margin: 0 5px -4px 0;
    background-color: rgba(0, 0, 0, 0);
  }

  .todo-item-cheked-input + label span:before,
  .todo-item-cheked-input + label span:after  {
    -webkit-transition: all 0.3s ease-in-out;
    -moz-transition: all 0.3s ease-in-out;
    transition: all 0.2s ease-in-out;
    content: "";
    position: absolute;
    z-index: 1;
    width: 1rem;
    height: 1rem;
    background: transparent;
    border: 2px solid var(--primary);
  }

  .todo-item-cheked-input + label span:after {
    z-index: 0;
    border: none;
  }

  .todo-item-cheked-input:checked + label span:before {
    z-index: 100;
    background-color: var(--primary);
  }

  .todo-item {
    display: flex;
    justify-content: start;
    align-items: center;
    width: min(80%, 30rem);

    margin: 0.5rem 0;
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
    justify-content: start;
    align-items: center;
  }

  .todo-item-name-edit {
    border: none;
  }

  .invisible {
    display: none;
  }
</style>
