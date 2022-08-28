mac:
	clang++ -dynamiclib -o ./bin/macOS/HelloWorld.bundle ./src/HelloWorld.cpp

android:
	ndk-build NDK_PROJECT_PATH=$${PWD} NDK_APPLICATION_MK="\$$(NDK_PROJECT_PATH)/Application.mk"