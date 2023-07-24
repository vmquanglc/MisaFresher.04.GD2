<template lang="">
  <div class="flex flex-col main-wrapper w-full h-full">
    <div class="content__header flex justify-between items-center pt-6 pb-6">
      <div class="content-header__title">
        <h3 class="font-24">{{ FreeText.directory }}</h3>
      </div>
    </div>
    <div class="content__body grid grid-cols-3 gap-24-0 flex-1 p-8">
      <div
        v-for="(directory, indexDirectory) in directoryList"
        :key="indexDirectory"
        class="">
        <div class="font-bold pb-2">{{ directory.title }}</div>
        <div
          v-for="(item, index) in directory.list"
          :key="index"
          class="pb-2"
          :title="item.subTitle">
          <router-link :to="{ name: item.name, param: {} }" class="text-blue">
            {{ item.title }}
          </router-link>
        </div>
      </div>
    </div>
  </div>
</template>
<script setup>
import { FreeText } from "@/resources";
import { ref } from "vue";
const directoryList = ref([
  {
    title: "Đối tượng",
    list: [
      {
        title: "Khách hàng",
        subTitle: "Khách hàng",
        // name: "DICustomner",
      },
      {
        title: "Nhà cung cấp",
        subTitle: "Nhà cung cấp",
        // name: "DISupplier",
      },
      {
        title: "Nhân viên",
        subTitle: "Nhân viên",
        // name: "DIEmployee",
      },
      {
        title: "Nhóm khách hàng, nhà cung cấp",
        subTitle: "Nhóm khách hàng, nhà cung cấp",
        // name: "DICustomnerSupplier",
      },
    ],
  },
  {
    title: "Vật tư, hàng hóa",
    list: [
      {
        title: "Vật tư, hàng hóa",
        subTitle: "Vật tư, hàng hóa",
        // name: "DIMaterialsGoods",
      },
      {
        title: "Kho",
        subTitle: "Kho",
        // name: "DIWarehouse",
      },
      {
        title: "Nhóm vật tư, hàng hóa, dịch vụ",
        subTitle: "Nhóm vật tư, hàng hóa, dịch vụ",
        // name: "DIMaterialsGoodsServices",
      },
      {
        title: "Đơn vị tính",
        subTitle: "Đơn vị tính",
        // name: "DIUnit",
      },
    ],
  },
  {
    title: "Account",
    list: [
      {
        title: "Hệ thống tài khoản",
        subTitle: "Hệ thống tài khoản",
        name: "DIAccountSystem",
      },
      {
        title: "Tài khoản kết chuyển",
        subTitle: "Tài khoản kết chuyển",
        // name: "DITranferAccount",
      },
      {
        title: "Tài khoản ngầm định",
        subTitle: "Tài khoản ngầm định",
        // name: "DIDefaultAccount",
      },
    ],
  },
]);
</script>
<style lang=""></style>
