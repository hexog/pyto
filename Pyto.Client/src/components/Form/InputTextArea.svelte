<script lang="ts">
  export let value = "";
  export let rows = 20;
  let input: HTMLTextAreaElement;

  export function focus() {
    input.focus();
  }
</script>

<textarea
  bind:value
  class="flex flex-grow outline-1 outline-none bg-nord-2 text-nord-6 w-full resize-none z-50 border-b-2 border-b-nord-6 py-2 px-3"
  bind:this={input}
  {rows}
/>
