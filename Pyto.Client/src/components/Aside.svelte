<script lang="ts">
    let isShrunk = true

    const changeSize = () => {
        isShrunk = !isShrunk
    }
</script>

<aside class="h-full bg-nord-1 text-nord-6 fixed w-3/12 { isShrunk ? '-translate-x-full' : '' } transition-all" on:click={changeSize}>

</aside>
