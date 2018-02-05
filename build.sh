# libeay32.dll        
# libgpr.dll   
# libgrpc++.dll    
# libstdc++-6.dll      
# ssleay32.dll
# libgcc_s_seh-1.dll  
# libgrpc.dll  
# libprotobuf.dll  
# libwinpthread-1.dll  
# zlib1.dll

clear
PATH=/mingw64/bin:$PATH
MIN_LIB=/mingw64/lib
MIN_BIN=/mingw64/bin
MIN_INCLUDE=/mingw64/include
PREFIX=x86_64-w64-mingw32
export C=$PREFIX-gcc.exe
export CC=$PREFIX-gcc.exe
export CXX=$PREFIX-g++.exe
export CPP=$PREFIX-cpp.exe
export RANLIB=$PREFIX-gcc-ranlib.exe
export CXXCPP=$PREFIX-cpp.exe
export AR=$PREFIX-ar.exe
export NM=$PREFIX-nm.exe
export DLLTOOL=$PREFIX-dlltool.exe
export DLLWRAP=$PREFIX-dllwrap.exe
export LD=$PREFIX-ld.exe
export OBJCOPY=$PREFIX-objcopy.exe
export OBJDUMP=$PREFIX-objdump.exe
export PKGCONFIG=$PREFIX-pkg-config.exe
echo
gcc --version
#echo
PATH_ROOT=$(pwd)
PATH_SRC=$(pwd)/src
PATH_PROTO=$(pwd)/proto
PATH_OBJ=$(pwd)/obj
echo PATH_ROOT: $PATH_ROOT
echo ----------------------------------------------------

rm test_protobuf.exe
rm $PATH_SRC/*
rm $PATH_ROOT/bin/*.exe

#///////////////////////////////////////////////////////////////////////////////

protoc \
	--proto_path=$PATH_PROTO \
	--grpc_out=$PATH_SRC \
	--plugin=protoc-gen-grpc=$MIN_BIN/grpc_cpp_plugin.exe \
	addressbook.proto
	
protoc \
	--proto_path=$PATH_PROTO \
	--cpp_out=$PATH_SRC \
	addressbook.proto
	
g++ -I$PATH_SRC -Wall \
	test_protobuf.cpp $PATH_SRC/addressbook.pb.cc \
	-lprotobuf -pthread -static-libgcc -static-libstdc++ -std=c++11 \
	-o ./bin/test_protobuf.exe

#///////////////////////////////////////////////////////////////////////////////
#-mwin32 -D_WIN32_WINNT=0x600	for win32
# route_guide

protoc \
	--proto_path=$PATH_PROTO \
	--grpc_out=$PATH_SRC/ \
	--plugin=protoc-gen-grpc=$MIN_BIN/grpc_cpp_plugin.exe \
	route_guide.proto
	
protoc \
	--proto_path=$PATH_PROTO \
	--cpp_out=$PATH_SRC/ \
	route_guide.proto

g++ -I$PATH_SRC -Wall -D_WIN32_WINNT=0x600 \
	route_guide_server.cc helper.cc \
	$PATH_SRC/route_guide.pb.cc $PATH_SRC/route_guide.grpc.pb.cc \
	-lprotobuf -lgrpc++ -pthread -static-libgcc -static-libstdc++ -std=c++11 \
	-o ./bin/route_server.exe
	
g++ -I$PATH_SRC -Wall -D_WIN32_WINNT=0x600 \
	route_guide_client.cc helper.cc \
	$PATH_SRC/route_guide.pb.cc $PATH_SRC/route_guide.grpc.pb.cc \
	-lprotobuf -lgrpc++ -pthread -static-libgcc -static-libstdc++ -std=c++11 \
	-o ./bin/route_client.exe
	
#///////////////////////////////////////////////////////////////////////////////
# helloworld

protoc \
	--proto_path=$PATH_PROTO \
	--grpc_out=$PATH_SRC/ \
	--plugin=protoc-gen-grpc=$MIN_BIN/grpc_cpp_plugin.exe \
	helloworld.proto
	
protoc \
	--proto_path=$PATH_PROTO \
	--cpp_out=$PATH_SRC/ \
	helloworld.proto

g++ -I$PATH_SRC -Wall -D_WIN32_WINNT=0x600 \
	greeter_server.cc \
	$PATH_SRC/helloworld.pb.cc $PATH_SRC/helloworld.grpc.pb.cc \
	-lprotobuf -lgrpc++ \
	-pthread -static-libgcc -static-libstdc++ -std=c++11 \
	-o ./bin/hello_server.exe
	
g++ -I$PATH_SRC -Wall -D_WIN32_WINNT=0x600 \
	greeter_client.cc \
	$PATH_SRC/helloworld.pb.cc $PATH_SRC/helloworld.grpc.pb.cc \
	-lprotobuf -lgrpc++ \
	-pthread -static-libgcc -static-libstdc++ -std=c++11 \
	-o ./bin/hello_client.exe

# libgpr.a: gpr_log > grpc/support/log.h

g++ -I$PATH_SRC -Wall -D_WIN32_WINNT=0x600 \
	greeter_async_server.cc \
	$PATH_SRC/helloworld.pb.cc $PATH_SRC/helloworld.grpc.pb.cc \
	-lprotobuf -lgrpc++ -lgpr \
	-pthread -static-libgcc -static-libstdc++ -std=c++11 \
	-o ./bin/hello_server_async.exe

g++ -I$PATH_SRC -Wall -D_WIN32_WINNT=0x600 \
	greeter_async_client2.cc \
	$PATH_SRC/helloworld.pb.cc $PATH_SRC/helloworld.grpc.pb.cc \
	-lprotobuf -lgrpc++ -lgpr \
	-pthread -static-libgcc -static-libstdc++ -std=c++11 \
	-o ./bin/hello_client_async2.exe

g++ -I$PATH_SRC -Wall -D_WIN32_WINNT=0x600 \
	greeter_async_client.cc \
	$PATH_SRC/helloworld.pb.cc $PATH_SRC/helloworld.grpc.pb.cc \
	-lprotobuf -lgrpc++ -lgpr \
	-pthread -static-libgcc -static-libstdc++ -std=c++11 \
	-o ./bin/hello_client_async.exe

echo
echo
echo --------------
echo BUILD COMPLETE
echo
