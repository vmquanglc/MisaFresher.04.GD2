import { ref, onMounted, onBeforeUnmount } from "vue";

// Hàm composable xử lý sự kiện click bên ngoài
export default function useClickOutside(elementRef) {
  const isOutside = ref(false);

  const handleClick = (event) => {
    if (elementRef.value && !elementRef.value.contains(event.target)) {
      isOutside.value = true;
    } else {
      isOutside.value = false;
    }
  };

  onMounted(() => {
    document.addEventListener("click", handleClick);
  });

  onBeforeUnmount(() => {
    document.removeEventListener("click", handleClick);
  });

  return isOutside;
}
