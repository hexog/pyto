<script lang="ts">
    export let id: string | null = null
    export let value = ''
</script>

<input type="text" {id} bind:value={value} class="bg-nord-2 px-3 h-8 rounded-sm shadow focus:outline-none">