const { defineConfig } = require("@vue/cli-service");

module.exports = defineConfig({
  transpileDependencies: true,
  productionSourceMap: true, // Enable source maps in production
  configureWebpack: {
    devtool: "source-map", // Set the devtool option to generate source maps
  },
});
