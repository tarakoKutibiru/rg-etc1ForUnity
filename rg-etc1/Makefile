mac:
	clang++ -dynamiclib -o ./bin/macOS/rg_etc1.bundle ./src/rg_etc1.cpp

android:
	ndk-build NDK_PROJECT_PATH=$${PWD} NDK_APPLICATION_MK="\$$(NDK_PROJECT_PATH)/Application.mk"