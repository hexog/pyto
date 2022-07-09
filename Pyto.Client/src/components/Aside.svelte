<script lang="ts">
    let isShrunk = true

    const changeSize = () => {
        isShrunk = !isShrunk
    }
</script>

<aside class="h-full bg-nord-1 text-nord-6 fixed { isShrunk ? 'w-16' : 'w-3/12' } transition-all" on:click={changeSize}>

</aside>
