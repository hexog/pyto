<script lang="ts">
  export let type: "primary" | "secondary" = "primary";
</script>

<div class="inline-block">
  <button
    class="flex justify-center items-center px-3 h-8 rounded-sm transition-colors {type ===
    'primary'
      ? 'text-nord-1 bg-nord-9 hover:bg-nord-10 active:bg-nord-8'
      : type === 'secondary'
      ? 'hover:bg-nord-9 active:bg-nord-8'
      : ''}"
    on:click|stopPropagation
  >
    <slot />
  </button>
</div>
