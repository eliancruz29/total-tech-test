<template>
  <div id="app">
    <el-container style="min-height: 100vh">
      <el-header style="background-color: #409eff; color: white; display: flex; align-items: center;">
        <h2 style="margin: 0;">Requirements Analyzer - AI Powered</h2>
      </el-header>
      <el-main>
        <router-view />
      </el-main>
    </el-container>
  </div>
</template>

<script setup>
</script>

<style>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

body {
  font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif;
}

#app {
  width: 100%;
}
</style>

