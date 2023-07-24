<template lang="">
  <div class="loading-wrapper">
    <div class="loading-container"></div>
  </div>
</template>
<script>
export default {};
</script>
<style>
.loading-wrapper {
  position: fixed;
  top: 0;
  right: 0;
  bottom: 0;
  left: 0;
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: rgba(0, 0, 0, 0.3);
  z-index: 999;
}
.loading-container {
  background: url(@/assets/images/loading.svg) no-repeat;
  width: 32px;
  height: 32px;
  animation: loading linear 0.5s infinite;
}
@keyframes loading {
  from {
    transform: rotate(0);
  }
  to {
    transform: rotate(360deg);
  }
}
</style>
