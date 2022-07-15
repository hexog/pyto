<script lang="ts">
  import type { TodoModel, TodoState } from "src/lib/TodoListService";
  import { createEventDispatcher } from "svelte";
  import ArrowDown from "../assets/arrow-down.svg";
  import Button from "./Form/Button.svelte";
  import { Remarkable } from "remarkable";

  const md = new Remarkable();

  /// Todo state
  export let id: string;
  export let content: string;
  export let state: TodoState;
  let isChecked = state === "checked";

  const getTodoModel = (): TodoModel => {
    return {
      id,
      content,
      state: isChecked ? "checked" : "unchecked",
    };
  };

  const dispatch = createEventDispatcher();

  //#region Editing
  let isEditing = false;
  let uneditedText = "";
  let editInput: HTMLTextAreaElement;
  let lines = 1;
  $: lines = content.split("\n").length;

  const startEditing = () => {
    changeViewMode("full");

    isEditing = true;
    uneditedText = content;
    setTimeout(() => {
      editInput.focus();
    }, 0);
  };

  const stopEditing = (confirm: boolean = true) => {
    console.log(isEditing);
    isEditing = false;

    if (!confirm) {
      content = uneditedText;
    } else {
      content = content.trim();
      dispatchTodoChange();
    }
  };

  const changeEditingState = () => {
    if (isEditing) {
      stopEditing();
    } else {
      startEditing();
    }
  };

  const dispatchTodoChange = () => {
    const todo: TodoModel = getTodoModel();
    dispatch("todochange", todo);
  };

  //#endregion

  //#region View mode

  type ViewMode = "full" | "compact";
  let fullViewMode: ViewMode = "compact";

  let isCompact = false;
  $: isCompact = fullViewMode === "compact";

  let isHighlighted = false;
  $: isHighlighted = !isCompact || isEditing;

  let canBeFullView = false;
  $: canBeFullView = content.indexOf("\n") !== -1 || content.length > 40;

  const getFullView = () => {
    return md.render(content);
  };

  let viewContent: string = "";
  $: viewContent = isCompact
    ? md.render(
        content
          .split("\n")[0]
          .replace(/^#{1,6} /i, "")
          .substring(0, 40)
      )
    : md.render(content);

  const changeViewMode = (state?: ViewMode) => {
    if (isEditing) {
      stopEditing(false);
    }

    if (!canBeFullView) {
      fullViewMode = "compact";
    } else {
      fullViewMode = state ?? (fullViewMode === "full" ? "compact" : "full");
    }
  };

  const changeViewModeWhenNotSelectingText = (event: MouseEvent) => {
    if (window.getSelection().toString()) {
      event.preventDefault();
    } else {
      changeViewMode();
    }
  };

  //#endregion
</script>

<div
  class="flex flex-row items-start gap-x-1 rounded py-2 px-3 hover:bg-nord-2 transition-all cursor-pointer h-fit {isHighlighted
    ? 'bg-nord-1 h-auto'
    : 'h-12'} {isEditing ? 'bg-nord-2' : ''}"
  on:click={changeViewModeWhenNotSelectingText}
>
  <div class="flex items-center justify-center h-[2.10rem]">
    <input
      class="todo-check"
      type="checkbox"
      {id}
      bind:checked={isChecked}
      on:change={() => dispatchTodoChange()}
    />
    <label for={id}><span /></label>
  </div>
  <div
    class="flex flex-col flex-grow justify-center overflow-auto m-1 transition-all"
  >
    {#if !isEditing}
      <div
        class="flex flex-col {isChecked && isCompact
          ? 'line-through text-nord-8'
          : ''}"
        id="content"
      >
        {@html viewContent}
      </div>
    {:else}
      <textarea
        on:click|stopPropagation
        type="text"
        bind:value={content}
        bind:this={editInput}
        class="flex flex-grow outline-1 outline-none bg-nord-2 text-nord-6 w-full resize-none z-50 border-b-2 border-b-nord-6"
        class:invisible={!isEditing}
        rows={lines}
      />
    {/if}
  </div>

  <div class="flex flex-col gap-y-1">
    <Button color="secondary" on:click={changeEditingState}>
      {#if isEditing}
        <div class="flex h-4 w-5 -mx-1 items-center justify-center">
          <svg
            width="24px"
            height="24px"
            viewBox="0 0 24 24"
            class="fill-nord-6"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path
              d="M9.172 18.657a1 1 0 0 1-.707-.293l-5.657-5.657a1 1 0 0 1 1.414-1.414l4.95 4.95L19.778 5.636a1 1 0 0 1 1.414 1.414L9.879 18.364a1 1 0 0 1-.707.293z"
            />
          </svg>
        </div>
      {:else}
        <div class="flex h-4 w-5 -mx-1 items-center justify-center">
          <svg
            version="1.1"
            id="Layer_1"
            xmlns="http://www.w3.org/2000/svg"
            xmlns:xlink="http://www.w3.org/1999/xlink"
            x="0px"
            y="0px"
            viewBox="0 0 512 512"
            style="enable-background:new 0 0 512 512;"
            class="fill-nord-6"
            xml:space="preserve"
            ><g
              ><g
                ><path
                  d="M387.182,0L0,387.181V512h124.818L512,124.819L387.182,0z M104.879,463.858H48.142v-56.735l282.303-282.303l56.735,56.735L104.879,463.858z M364.486,90.78l22.694-22.694l56.737,56.734l-22.696,22.696L364.486,90.78z"
                /></g
              ></g
            ><g /><g /><g /><g /><g /><g /><g /><g /><g /><g /><g /><g /><g /><g
            /><g /></svg
          >
        </div>
      {/if}
    </Button>
    {#if isHighlighted && canBeFullView}
      <Button
        color="secondary"
        on:click={() => dispatch("tododelete", getTodoModel())}
      >
        <div class="flex h-5 w-5 -mx-1 items-center justify-center">
          <svg
            width="24px"
            height="24px"
            class="fill-nord-6"
            viewBox="0 0 24 24"
            fill="none"
            xmlns="http://www.w3.org/2000/svg"
            ><path
              d="M7 4a2 2 0 0 1 2-2h6a2 2 0 0 1 2 2v2h4a1 1 0 1 1 0 2h-1.069l-.867 12.142A2 2 0 0 1 17.069 22H6.93a2 2 0 0 1-1.995-1.858L4.07 8H3a1 1 0 0 1 0-2h4V4zm2 2h6V4H9v2zM6.074 8l.857 12H17.07l.857-12H6.074zM10 10a1 1 0 0 1 1 1v6a1 1 0 1 1-2 0v-6a1 1 0 0 1 1-1zm4 0a1 1 0 0 1 1 1v6a1 1 0 1 1-2 0v-6a1 1 0 0 1 1-1z"
            /></svg
          >
        </div>
      </Button>
    {/if}
  </div>
  {#if canBeFullView}
    <Button color="secondary" on:click={changeViewModeWhenNotSelectingText}>
      {#if isEditing}
        <div class="flex h-5 w-3 items-center justify-center">
          <svg
            version="1.1"
            id="Capa_1"
            xmlns="http://www.w3.org/2000/svg"
            xmlns:xlink="http://www.w3.org/1999/xlink"
            class="fill-nord-6"
            x="0px"
            y="0px"
            viewBox="0 0 490.29 490.29"
            style="enable-background:new 0 0 490.29 490.29;"
            xml:space="preserve"
          >
            <g>
              <g>
                <g>
                  <rect
                    x="206.343"
                    y="-62.678"
                    transform="matrix(-0.7071 0.7071 -0.7071 -0.7071 591.6399 245.173)"
                    width="77.399"
                    height="615.594"
                  />

                  <rect
                    x="-9.144"
                    y="335.976"
                    transform="matrix(0.7071 -0.7071 0.7071 0.7071 -231.1054 191.4143)"
                    width="249.298"
                    height="77.399"
                  />

                  <rect
                    x="250.136"
                    y="77.228"
                    transform="matrix(-0.7071 0.7071 -0.7071 -0.7071 721.7715 -67.1118)"
                    width="249.298"
                    height="77.399"
                  />
                </g>
              </g>
            </g>
            <g />
            <g />
            <g />
            <g />
            <g />
            <g />
            <g />
            <g />
            <g />
            <g />
            <g />
            <g />
            <g />
            <g />
            <g />
          </svg>
        </div>
      {:else}
        <div class="flex h-5 w-5 -mx-1 items-center justify-center">
          {#if canBeFullView}
            <svg
              version="1.1"
              id="Layer_1"
              xmlns="http://www.w3.org/2000/svg"
              xmlns:xlink="http://www.w3.org/1999/xlink"
              x="0px"
              y="0px"
              viewBox="0 0 330 330"
              style="enable-background:new 0 0 330 330;"
              xml:space="preserve"
              class="fill-nord-6 {isHighlighted
                ? 'rotate-180'
                : ''} transition-transform"
              ><path
                id="XMLID_225_"
                d="M325.607,79.393c-5.857-5.857-15.355-5.858-21.213,0.001l-139.39,139.393L25.607,79.393	c-5.857-5.857-15.355-5.858-21.213,0.001c-5.858,5.858-5.858,15.355,0,21.213l150.004,150c2.813,2.813,6.628,4.393,10.606,4.393	s7.794-1.581,10.606-4.394l149.996-150C331.465,94.749,331.465,85.251,325.607,79.393z"
              /><g /><g /><g /><g /><g /><g /><g /><g /><g /><g /><g /><g /><g
              /><g /><g /></svg
            >
          {/if}
        </div>
      {/if}
    </Button>
  {:else if isEditing}
    <Button color="secondary" on:click={changeViewModeWhenNotSelectingText}>
      <div class="flex h-5 w-3 items-center justify-center">
        <svg
          version="1.1"
          id="Capa_1"
          xmlns="http://www.w3.org/2000/svg"
          xmlns:xlink="http://www.w3.org/1999/xlink"
          class="fill-nord-6"
          x="0px"
          y="0px"
          viewBox="0 0 490.29 490.29"
          style="enable-background:new 0 0 490.29 490.29;"
          xml:space="preserve"
        >
          <g>
            <g>
              <g>
                <rect
                  x="206.343"
                  y="-62.678"
                  transform="matrix(-0.7071 0.7071 -0.7071 -0.7071 591.6399 245.173)"
                  width="77.399"
                  height="615.594"
                />

                <rect
                  x="-9.144"
                  y="335.976"
                  transform="matrix(0.7071 -0.7071 0.7071 0.7071 -231.1054 191.4143)"
                  width="249.298"
                  height="77.399"
                />

                <rect
                  x="250.136"
                  y="77.228"
                  transform="matrix(-0.7071 0.7071 -0.7071 -0.7071 721.7715 -67.1118)"
                  width="249.298"
                  height="77.399"
                />
              </g>
            </g>
          </g>
          <g />
          <g />
          <g />
          <g />
          <g />
          <g />
          <g />
          <g />
          <g />
          <g />
          <g />
          <g />
          <g />
          <g />
          <g />
        </svg>
      </div>
    </Button>
  {:else}
    <Button
      color="secondary"
      on:click={() => dispatch("tododelete", getTodoModel())}
    >
      <div class="flex h-5 w-5 -mx-1 items-center justify-center">
        <svg
          width="24px"
          height="24px"
          class="fill-nord-6"
          viewBox="0 0 24 24"
          fill="none"
          xmlns="http://www.w3.org/2000/svg"
          ><path
            d="M7 4a2 2 0 0 1 2-2h6a2 2 0 0 1 2 2v2h4a1 1 0 1 1 0 2h-1.069l-.867 12.142A2 2 0 0 1 17.069 22H6.93a2 2 0 0 1-1.995-1.858L4.07 8H3a1 1 0 0 1 0-2h4V4zm2 2h6V4H9v2zM6.074 8l.857 12H17.07l.857-12H6.074zM10 10a1 1 0 0 1 1 1v6a1 1 0 1 1-2 0v-6a1 1 0 0 1 1-1zm4 0a1 1 0 0 1 1 1v6a1 1 0 1 1-2 0v-6a1 1 0 0 1 1-1z"
          /></svg
        >
      </div>
    </Button>
  {/if}
</div>

<style lang="postcss" scoped>
  .todo-check {
    display: none;
  }

  .todo-check + label span {
    display: block;
    width: 25px;
    height: 19px;
    background-color: rgba(0, 0, 0, 0);
    cursor: pointer;
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
    @apply border-2 border-nord-9;
  }

  .todo-check + label span:after {
    z-index: 0;
    border: none;
  }

  .todo-check:checked + label span:before {
    z-index: 100;
    @apply bg-nord-9;
  }

  .invisible {
    display: none;
  }

  :global(#content h1) {
    @apply text-4xl font-semibold;
    @apply mb-1 -mt-1.5;
  }

  :global(#content * ~ h1) {
    @apply mt-2;
  }

  :global(#content h2) {
    @apply text-3xl font-semibold;
  }

  :global(#content h3) {
    @apply text-2xl font-semibold;
    @apply text-nord-11;
  }

  :global(#content h4) {
    @apply text-xl font-bold;
    @apply text-nord-12;
  }

  :global(#content h5) {
    @apply text-lg font-bold;
    @apply text-nord-13;
  }

  :global(#content h6) {
    @apply font-bold;
  }

  :global(strong) {
    @apply text-nord-14;
  }

  :global(em) {
    @apply text-nord-7;
  }
</style>
