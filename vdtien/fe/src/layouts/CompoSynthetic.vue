<template>
  <div class="m-1">
    <div class="button">
      <h1>1.Button</h1>
      <div class="flex items-center justify-center">
        <BaseButton :button-type="ButtonType.pri" :title="`Button`" />
        <BaseButton :button-type="ButtonType.sub" :title="`Button`" />
        <BaseButton :button-type="ButtonType.link" :title="`Button`" />
        <BaseButton :button-type="ButtonType.icon" :title="`Button`" />
        <BaseButton :button-type="ButtonType.combo" :title="`Button`" />
        <BaseButton :button-type="ButtonType.wIcon" :title="`Button`" />
      </div>
    </div>
    <div class="textfield">
      <h1>2.Textfield</h1>
      <div>
        <BaseTextfield
          v-model="inputValue"
          label="Textfield"
          required
          :err-msg="errMsg"
          place-holder="Enter a value" />
      </div>
    </div>
    <div>{{ inputValue }}</div>

    <div class="dropdown">
      <h1>3.Dropdown</h1>
      <BaseDropdown
        label="Danh sách phân trang"
        required
        :data="dataDropdown"
        title-dropdown-list="--Chọn số bản ghi trên một trang--" />
    </div>
  </div>
</template>

<script>
import BaseButton from "@/components/bases/BaseButton.vue";
import BaseTextfield from "@/components/bases/BaseTextfield.vue";
import BaseDropdown from "@/components/bases/BaseDropdown.vue";
import { ButtonType } from "@/enums";
import { ref } from "vue";
export default {
  components: {
    BaseButton,
    BaseTextfield,
    BaseDropdown,
  },
  setup() {
    const inputValue = ref("1233");
    const errMsg = ref("");
    const dataDropdown = ref([
      { text: "10 bản ghi trên một trang", value: 10 },
      { text: "20 bản ghi trên một trang", value: 20 },
      { text: "50 bản ghi trên một trang", value: 50 },
      { text: "100 bản ghi trên một trang", value: 100 },
    ]);
    // console.log(inputValue);
    return {
      ButtonType,
      inputValue,
      errMsg,
      dataDropdown,
    };
  },
};
</script>

<style></style>
