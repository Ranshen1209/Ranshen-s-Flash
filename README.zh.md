## 刷机工具

该工具简化了安装新固件的流程，使其即使对于没有丰富技术背景的用户也容易上手。

### README 翻译版本

---

- [English](README.md)
- [中文](README.zh.md)

### 入门指南

---

以下说明将帮助您在本地计算机上获取项目副本，并为开发和测试做好准备。

### 先决条件

---

您需要安装以下软件并确保它们已正确安装：

- Windows 10 1803 或更高版本。
- 机器上必须安装 .NET Framework 4.7.2 或更高版本。

### 安装步骤

---

以下分步骤示例将指导您如何搭建开发环境：

1. 克隆仓库或下载 ZIP 文件。

```bash
git clone https://github.com/Ranshen1209/Ranshen-s-Flash.git
```

2. 进入克隆或下载的目录。

3. 使用 Visual Studio 构建项目。

4. 构建项目后，`flashbin` 目录将配置好操作所需的文件和文件夹。

```bash
/flashbin
|-- /platform-tools           # 包含 Google 的 adb 和 fastboot 可执行文件
|-- /resources                # 程序运行所需的内容
|-- /drivers                  # 包含设备驱动程序，特别是来自小米的驱动
```

### 如何应用

---

#### 安装驱动脚本示例

```bash
@echo off
"C:\Windows\sysnative\pnputil.exe" /add-driver "%~dp0..\ThirdParty\Google\Driver\android_winusb.inf" /install
"C:\Windows\sysnative\pnputil.exe" /add-driver "%~dp0..\ThirdParty\Qualcomm\Driver\qcser.inf" /install
```

#### 线刷脚本示例

```bash
@echo off
set PATH=%~dp0flashbin\platform-tools;%PATH%

REM 检查设备型号
fastboot %* getvar product 2>&1 | findstr /r /c:"^product: *marble" || (
    echo Missmatching image and device
    exit /B 1
)

REM 擦除boot_ab分区
fastboot %* erase boot_ab || (
    echo "Erase boot error"
    exit /B 1
)

REM 定义要刷写的镜像和对应的分区
set images=abl_ab xbl_ab xbl_config_ab shrm_ab aop_ab aop_config_ab tz_ab devcfg_ab featenabler_ab hyp_ab uefi_ab uefisecapp_ab modem_ab bluetooth_ab dsp_ab keymaster_ab qupfw_ab cpucp_ab rescue xbl_ramdump_ab imagefv_ab vendor_boot_ab dtbo_ab vbmeta_ab vbmeta_system_ab cust recovery_ab boot_ab
set files=abl.img xbl.img xbl_config.img shrm.img aop.img aop_config.img tz.img devcfg.img featenabler.img hyp.img uefi.img uefisecapp.img modem.img bluetooth.img dsp.img keymaster.img qupfw.img cpucp.img rescue.img xbl_ramdump.img imagefv.img vendor_boot.img dtbo.img vbmeta.img vbmeta_system.img cust.img recovery.img boot.img

REM 循环刷写镜像
for %%i in (%images%) do (
    for %%j in (%files%) do (
        fastboot %* flash %%i %~dp0images/%%j || (
            echo "Flash %%i error"
            exit 1
        )
    )
)

REM 擦除并刷写imagefv_ab分区
fastboot %* erase imagefv_ab || (
    echo "Erase imagefv error"
    exit /B 1
)
fastboot %* flash imagefv_ab %~dp0images/imagefv.img || (
    echo "Flash imagefv error"
    exit 1
)

REM 单独刷写super.img和misc.img
fastboot %* flash super %~dp0super.img || (
    echo "Flash super error"
    exit 1
)
fastboot %* flash misc %~dp0images\misc.img || (
    echo "Flash misc error"
    exit /B 1
)

REM 刷写vbmeta.img和vbmeta_system.img并禁用校验
fastboot %* --disable-verity --disable-verification flash vbmeta %~dp0images/vbmeta.img || (
    echo "Flash vbmeta error"
    exit 1
)
fastboot %* --disable-verity --disable-verification flash vbmeta_system %~dp0images/vbmeta_system.img || (
    echo "Flash vbmeta_system error"
    exit 1
)

REM 设置活动分区为a
fastboot %* set_active a || (
    echo "Set active a error"
    exit 1
)

REM 重启设备
fastboot %* reboot || (
    echo "Reboot error"
    exit 1
)

```

#### 卡刷脚本示例

```shell
#!/sbin/sh
set -e
ZIP=$3
OUTFD=/proc/self/fd/$2

ui_print(){
    echo -e "ui_print $1\nui_print" > $OUTFD
}

package_extract_file(){
    if unzip -l "$ZIP" "$1" >/dev/null 2>&1; then
        unzip -p "$ZIP" "$1" > "$2"
    else
        ui_print "Error: Cannot find $1 in $ZIP"
        exit 1
    fi
}

PartitionFilePath=/dev/block/by-name

flash_partition() {
    local partition=$1
    ui_print "Flashing $partition partition"
    package_extract_file "images/$partition.img" "${PartitionFilePath}/${partition}_a"
    package_extract_file "images/$partition.img" "${PartitionFilePath}/${partition}_b"
}

for partition in abl xbl xbl_config shrm aop aop_config tz devcfg featenabler hyp uefi uefisecapp modem bluetooth dsp keymaster qupfw cpucp xbl_ramdump imagefv vendor_boot dtbo vbmeta vbmeta_system recovery; do
    flash_partition $partition
done

ui_print "Flashing rescue partition"
package_extract_file "images/rescue.img" "${PartitionFilePath}/rescue"

ui_print "Flashing super partition"
package_extract_file "super.img" "${PartitionFilePath}/super"

ui_print "Flashing cust partition"
package_extract_file "images/cust.img" "${PartitionFilePath}/cust"

ui_print "Flashing misc partition"
package_extract_file "images/misc.img" "${PartitionFilePath}/misc"

exit 0
```

