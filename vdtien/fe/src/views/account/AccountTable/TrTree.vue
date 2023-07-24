<template>
  <template v-for="item in treeData" :key="item.AccountId">
    <tr class="pointer">
      <td
        v-for="(col, key, indexCol) in colsName"
        :key="indexCol"
        :class="{
          'td-anchor td-anchor--end td-action': key === 'action',
          'before-last': indexCol === Object.keys(colsName).length - 2,
          'font-bold': item?.IsParent,
        }"
        :style="{
          'z-index':
            rowSelected === item?.AccountId && key === 'action' ? 1 : 0,
        }">
        <span
          v-if="key == 'AccountCode'"
          class="flex items-center gap-0-8"
          :style="{ 'padding-left': `${24 * item?.Grade ?? 0}px` }">
          <div class="w-4 h-4 flex items-center justify-center">
            <div
              class="icon"
              :class="{
                'icon--plus-square': item?.IsParent && !item?.showChild,
                'icon--minus-square': item?.IsParent && item?.showChild,
              }"
              @click="() => onClickToggleShowChild(item)"></div>
          </div>
          <span>{{ item[key] }}</span>
        </span>
        <span v-else-if="key === 'AccountFeature'">
          {{ converAccountFeature(item[key]) }}</span
        >
        <span v-else-if="key === 'Status'">
          {{ converStatusField(item[key]) }}
        </span>

        <div
          v-else-if="key === 'action'"
          class="flex items-center justify-center h-full"
          @dblclick.stop=""
          @click.stop="">
          <a
            href="#"
            class="text-blue font-semi-bold h-full flex items-center px-4"
            @click="() => onOpenPopupUpdate(item)"
            >{{ ButtonTitle.edit }}</a
          >

          <div
            class="td-action__icon"
            :style="{
              'z-index':
                rowSelected === item.AccountId && key === 'action' ? 2 : 0,
            }">
            <div
              class="icon-wrapper w-8 h-8"
              :class="{
                'border--blue': rowSelected === item.AccountId,
              }"
              @click="(e) => hanldeEventClickMoreAcion(e, item)">
              <div class="icon icon--down-small-blue"></div>
            </div>

            <div
              v-if="rowSelected === item.AccountId"
              class="dropdown-list td-action-list"
              :class="{ 'td-action-list--up': directUpDropdown }">
              <div
                class="dropdown-item td-action-item td-action-item--remove"
                @click="() => onClickDeleteEmployee(item)">
                {{ ButtonTitle.delete }}
              </div>
              <div
                class="dropdown-item td-action-item"
                @click="() => onClickCloneRecord(item)">
                {{ ButtonTitle.duplicate }}
              </div>
            </div>
          </div>
          <div
            v-if="rowSelected === item.AccountId"
            class="overlay"
            @click="() => toggleTableAction()" />
        </div>
        <span v-else> {{ item[key] }}</span>
      </td>
    </tr>
    <tr-tree
      v-if="item?.children && item?.showChild"
      :tree-data="item.children"
      :cols-name="colsName"
      :show-child="item.showChild"
      :row-selected="rowSelected"
      :position-table-bottom="positionTableBottom"
      :toggle-table-action="toggleTableAction">
    </tr-tree>
  </template>
</template>
<script>
import { ref } from "vue";
import { useStore } from "vuex";
import { ButtonTitle } from "@/resources";
import { converAccountFeature, converStatusField } from "@/utils/helper";
export default {
  name: "TrTree",
  props: {
    treeData: {
      type: Array,
      default: () => [],
    },
    colsName: {
      type: Object,
      default: () => ({}),
    },
    showChild: {
      type: Boolean,
      default: false,
    },
    toggleTableAction: {
      type: Function,
      default: () => {},
    },
    rowSelected: {
      type: String,
      default: "",
    },
    positionTableBottom: {
      type: Number,
      default: 0,
    },
  },
  setup(props) {
    const store = useStore();
    const directUpDropdown = ref(false);
    const hanldeEventClickMoreAcion = (e, item) => {
      props.toggleTableAction(item);
      // console.log(props.positionTableBottom - e.clientY);
      if (props.positionTableBottom - e.clientY < 100) {
        directUpDropdown.value = true;
      } else {
        directUpDropdown.value = false;
      }
    };

    const onClickToggleShowChild = (item) => {
      if (item?.showChild) {
        item.showChild = false;
      } else if (item.showChild === false) {
        if (item && !item?.children?.length) {
          store.dispatch("getAccountsListByParentId", item);
        }
        item.showChild = true;
      }
    };
    return {
      ButtonTitle,
      hanldeEventClickMoreAcion,
      directUpDropdown,
      converAccountFeature,
      onClickToggleShowChild,
      converStatusField,
    };
  },
};
</script>
<style></style>
