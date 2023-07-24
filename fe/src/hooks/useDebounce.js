// hooks/useDebounce.js
import { ref, watch } from "vue";

export default function useDebounce(value, delay) {
  const debouncedValue = ref(value.value);
  let timeoutId;

  watch(value, () => {
    clearTimeout(timeoutId);

    timeoutId = setTimeout(() => {
      debouncedValue.value = value.value;
    }, delay);
  });

  return debouncedValue;
}
