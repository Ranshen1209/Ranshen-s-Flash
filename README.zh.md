## KittenHyper 刷机工具

该工具专为 KittenHyper 项目而开发。它简化了安装新固件的流程，使其即使对于没有丰富技术背景的用户也容易上手。

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

#### KittenHyper_Stable 目录结构

```bash
KittenHyper_Stable
├── flashbin
│   ├── platform-tools    # adb和fastboot环境
│   │   ├── adb.exe
│   │   ├── AdbWinApi.dll
│   │   ├── AdbWinUsbApi.dll
│   │   ├── etc1tool.exe
│   │   ├── fastboot.exe
│   │   ├── hprof-conv.exe
│   │   ├── libwinpthread-1.dll
│   │   ├── make_f2fs.exe
│   │   ├── make_f2fs_casefold.exe
│   │   ├── mke2fs.conf
│   │   ├── mke2fs.exe
│   │   ├── NOTICE.txt
│   │   ├── source.properties
│   │   └── sqlite3.exe
│   ├── resources    # Ranshen's Flash所需资源
│   │   ├── android_FILL0.ico
│   │   └── install_drivers.bat
│   └── thirdparty    # 驱动目录
│       ├── Google
│       │   ├── Android
│       │   │   ├── adb.exe
│       │   │   ├── AdbWinApi.dll
│       │   │   ├── AdbWinUsbApi.dll
│       │   │   ├── fastboot.exe
│       │   │   ├── libwinpthread-1.dll
│       │   │   ├── make_f2fs.exe
│       │   │   ├── mke2fs.exe
│       │   │   ├── NOTICE.txt
│       │   │   ├── O_adb.exe
│       │   │   ├── O_AdbWinApi.dll
│       │   │   ├── O_AdbWinUsbApi.dll
│       │   │   ├── o_fastboot.exe
│       │   │   └── source.properties
│       │   └── Driver
│       │       ├── amd64
│       │       │   ├── NOTICE.txt
│       │       │   ├── WdfCoInstaller01009.dll
│       │       │   ├── winusbcoinstaller2.dll
│       │       │   └── WUDFUpdate_01009.dll
│       │       ├── android_winusb.inf
│       │       ├── i386
│       │       │   ├── NOTICE.txt
│       │       │   ├── WdfCoInstaller01009.dll
│       │       │   ├── winusbcoinstaller2.dll
│       │       │   └── WUDFUpdate_01009.dll
│       │       ├── source.properties
│       │       ├── xiaomiwinusb86.cat
│       │       └── xiaomiwinusba64.cat
│       └── Qualcomm
│           ├── Driver
│           │   ├── qcser.cat
│           │   ├── qcser.inf
│           │   └── serial
│           │       ├── amd64
│           │       │   ├── qcCoInstaller.dll
│           │       │   └── qcusbser.sys
│           │       ├── arm
│           │       │   └── qcusbser.sys
│           │       └── i386
│           │           ├── qcCoInstaller.dll
│           │           └── qcusbser.sys
│           └── fh_loader
│               ├── ffutoraw.exe
│               ├── fh_loader.c
│               ├── fh_loader.exe
│               ├── lsusb.exe
│               ├── QSaharaServer
│               │   └── src
│               │       ├── comm.c
│               │       ├── comm.h
│               │       ├── common_protocol_defs.h
│               │       ├── external_utils.c
│               │       ├── external_utils.h
│               │       ├── kickstart.c
│               │       ├── kickstart_log.c
│               │       ├── kickstart_log.h
│               │       ├── kickstart_utils.c
│               │       ├── kickstart_utils.h
│               │       ├── Makefile
│               │       ├── sahara_protocol.c
│               │       ├── sahara_protocol.h
│               │       ├── windows_utils.c
│               │       └── windows_utils.h
│               └── QSaharaServer.exe
├── flash_all.bat
├── flash_all_except_storage.bat
├── images    # 系统镜像
│   ├── abl.img
│   ├── aop.img
│   ├── aop_config.img
│   ├── bluetooth.img
│   ├── boot.img
│   ├── cpucp.img
│   ├── cust.img
│   ├── devcfg.img
│   ├── dsp.img
│   ├── dtbo.img
│   ├── dummy.img
│   ├── featenabler.img
│   ├── hyp.img
│   ├── imagefv.img
│   ├── keymaster.img
│   ├── metadata.img
│   ├── misc.img
│   ├── modem.img
│   ├── persist.img
│   ├── qupfw.img
│   ├── recovery.img
│   ├── rescue.img
│   ├── shrm.img
│   ├── tz.img
│   ├── uefi.img
│   ├── uefisecapp.img
│   ├── userdata.img
│   ├── vbmeta.img
│   ├── vbmeta_system.img
│   ├── vendor_boot.img
│   ├── xbl.img
│   ├── xbl_config.img
│   └── xbl_ramdump.img
├── META-INF    # 卡刷脚本
│   └── com
│       └── google
│           └── android
│               └── update-binary
├── Ranshen's Flash.exe
└── super.img
```

#### 安装驱动脚本示例

```bash
@echo off
"C:\Windows\sysnative\pnputil.exe" /add-driver "%~dp0..\ThirdParty\Google\Driver\android_winusb.inf" /install
"C:\Windows\sysnative\pnputil.exe" /add-driver "%~dp0..\ThirdParty\Qualcomm\Driver\qcser.inf" /install
```

#### 线刷脚本示例

```bash
set PATH=%~dp0flashbin\platform-tools;%PATH%
fastboot %* getvar product 2>&1 | findstr /r /c:"^product: *marble" || echo Missmatching image and device
fastboot %* getvar product 2>&1 | findstr /r /c:"^product: *marble" || exit /B 1
fastboot %* erase boot_ab || @echo "Erase boot error" && exit /B 1
fastboot %* flash abl_ab %~dp0images/abl.img || @echo "Flash abl error" && exit 1
fastboot %* flash xbl_ab %~dp0images/xbl.img || @echo "Flash xbl error" && exit 1
fastboot %* flash xbl_config_ab %~dp0images/xbl_config.img || @echo "Flash xbl_config error" && exit 1
fastboot %* flash shrm_ab %~dp0images/shrm.img || @echo "Flash shrm error" && exit 1
fastboot %* flash aop_ab %~dp0images/aop.img || @echo "Flash aop error" && exit 1
fastboot %* flash aop_config_ab %~dp0images/aop_config.img || @echo "Flash aop_config error" && exit 1
fastboot %* flash tz_ab %~dp0images/tz.img || @echo "Flash tz error" && exit 1
fastboot %* flash devcfg_ab %~dp0images/devcfg.img || @echo "Flash devcfg error" && exit 1
fastboot %* flash featenabler_ab %~dp0images/featenabler.img || @echo "Flash featenabler error" && exit 1
fastboot %* flash hyp_ab %~dp0images/hyp.img || @echo "Flash hyp error" && exit 1
fastboot %* flash uefi_ab %~dp0images/uefi.img || @echo "Flash uefi error" && exit 1
fastboot %* flash uefisecapp_ab %~dp0images/uefisecapp.img || @echo "Flash uefisecapp error" && exit 1
fastboot %* flash modem_ab %~dp0images/modem.img || @echo "Flash modem error" && exit 1
fastboot %* flash bluetooth_ab %~dp0images/bluetooth.img || @echo "Flash bluetooth error" && exit 1
fastboot %* flash dsp_ab %~dp0images/dsp.img || @echo "Flash dsp error" && exit 1
fastboot %* flash keymaster_ab %~dp0images/keymaster.img || @echo "Flash keymaster error" && exit 1
fastboot %* flash qupfw_ab %~dp0images/qupfw.img || @echo "Flash qupfw error" && exit 1
fastboot %* flash cpucp_ab %~dp0images/cpucp.img || @echo "Flash cpucp error" && exit 1
fastboot %* flash rescue %~dp0images/rescue.img || @echo "Flash rescue error" && exit 1
fastboot %* flash xbl_ramdump_ab %~dp0images/xbl_ramdump.img || @echo "Flash xbl_ramdump error" && exit 1
fastboot %* erase imagefv_ab || @echo "Erase imagefv error" && exit /B 1
fastboot %* flash imagefv_ab %~dp0images/imagefv.img || @echo "Flash imagefv error" && exit 1
fastboot %* flash super %~dp0super.img || @echo "Flash super error" && exit 1
fastboot %* flash vendor_boot_ab %~dp0images/vendor_boot.img || @echo "Flash vendor_boot error" && exit 1
fastboot %* flash dtbo_ab %~dp0images/dtbo.img || @echo "Flash dtbo error" && exit 1
fastboot %* flash vbmeta_ab %~dp0images/vbmeta.img || @echo "Flash vbmeta error" && exit 1
fastboot %* flash vbmeta_system_ab %~dp0images/vbmeta_system.img || @echo "Flash vbmeta_system error" && exit 1
fastboot %* flash cust %~dp0images/cust.img || @echo "Flash cust error" && exit 1
fastboot %* flash recovery_ab %~dp0images/recovery.img || @echo "Flash recovery error" && exit 1
fastboot %* flash boot_ab %~dp0images/boot.img || @echo "Flash boot error" && exit 1
fastboot %* flash misc %~dp0images\misc.img || @echo "Flash misc error" && exit /B 1
fastboot %* --disable-verity --disable-verification flash vbmeta %~dp0images/vbmeta.img || @echo "Flash vbmeta error" && exit 1
fastboot %* --disable-verity --disable-verification flash vbmeta_system %~dp0images/vbmeta_system.img || @echo "Flash vbmeta_system error" && exit 1
fastboot %* set_active a || @echo "Set active a error" && exit 1
fastboot %* reboot || @echo "Reboot error" && exit 1
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

