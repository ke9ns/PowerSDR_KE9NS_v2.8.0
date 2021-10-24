c--------------------------------------------------------------
         winapp 80000,120000
c# anttyp90.for
C***********************************************************************
      PROGRAM anttyp90
*    +               (run_directory,mode)
      include <windows.ins>
C***********************************************************************
c Calculate an exteranl antenna file.
c Assumes data is in the format of:
c       ..\antennas\samples\sample.90
c Execute with:
c    anttyp90 directory mode
c where:
c    directory = full pathname to the RUN directory (e.g. c:\ITSHFBC\RUN)
c    mode      = (blank) = Point-to-Point
c              = a = Area Coverage
C***********************************************************************
      common /Cant90/ luaa,filenam,title,itype,parms(20),
     +                 nfreq,frequency(100),dbi(100),eff(100),
     +                 ifreq1,gain1(91,360),ifreq2,gain2(91,360)
         character filenam*80,title*80
      common /cantenna/ anttype,antname,
     +                  xfqs,xfqe,designfreq,antfile,
     +                  beammain,offazim,cond,diel,
     +                  gain(91)
      character anttype*10,antname*70,antfile*24

      character cmnam*64,win_title*80,filename*80,gainfile*80
      integer*4 window_handle
      integer*2 x_pos,y_pos,xsize,ysize
      common /crun_directory/ run_directory
         character run_directory*50
      character mode*1
      
C.....START OF PROGRAM
c****************************************************************
ccc      open(66,file='\itshfbc\run\anttyp90.dmp')
ccc      rewind(66)
c****************************************************************
      run_directory=cmnam()
      nch_run=lenchar(run_directory)
      if(nch_run.lt.3) go to 930
      call ucase(run_directory,nch_run)
      mode=cmnam()
c****************************************************************
ccc      write(66,'('' run_directory='',a)') run_directory(1:nch_run)
ccc      write(66,'('' mode='',a)') mode
c****************************************************************
      iquiet=0
      if(iquiet.eq.0) then
         win_title='External Antenna Program ANTTYP90 output'
         xsize=GetSystemMetrics(SM_CXSCREEN)
         ysize=GetSystemMetrics(SM_CYSCREEN)/3
         x_pos=0
         y_pos=0
         window_handle=create_window(win_title,x_pos,y_pos,xsize,ysize)
         ier=set_default_window@(window_handle)
      end if
c****************************************************************
      open(21,file=run_directory(1:nch_run)//'\anttyp90.dat',
     +        status='old',err=900)
      rewind(21)
      read(21,*,err=920) idx          !  antenna index #, GAINxx.dat
      read(21,'(a)',err=920) antfile  !  antenna file name
      read(21,*,err=920) xfqs         !  starting frequency
      read(21,*,err=920) xfqe         !  ending frequency
      read(21,*,err=920) beammain     !  main beam (deg from North)
      read(21,*,err=920) offazim      !  off azimuth (deg from North)
      close(21)
      nch=lenchar(antfile)
      filename=run_directory(1:nch_run-3)//'antennas\'//antfile(1:nch)
ccc      write(66,'('' filename='',a)') filename
ccc      write(66,*) 'data=',idx,xfqs,xfqe,beammain,offazim
      lua=42
      call ant90_read(filename,21,lua,*910)
      diel=parms(3)         !  dielectric constant
      cond=parms(4)         !  conductivity
      write(gainfile,1) run_directory(1:nch_run),idx
1     format(a,5h\gain,i2.2,4h.dat)
      open(22,file=gainfile)
      rewind(22)
      write(22,'(a)') 'Extern#90/'//title
c****************************************************************
      if(mode.ne.' ') go to 200     !  area coverage
c          Point-to-Point mode
      write(22,2) xfqs,xfqe,beammain,offazim,cond,diel
2     format(2f5.0,2f7.2,2f10.5)
      azimuth=offazim
      do 50 ifreq=1,30
      freq=ifreq
      if(freq.ge.xfqs .and. freq.le.xfqe) then    !  in frequency range
         do 20 iel=0,90
         elev=iel
20       call ant90_calc(freq,azimuth,elev,gain(iel+1),aeff,*940)
      else                                        !  outside freq range
         aeff=0.
         do 30 iel=0,90
30       gain(iel+1)=0.
      end if
      write(22,3) ifreq,aeff,gain
3     format(i2,f6.2,(T10,10F7.3))
50    continue
      go to 500
c****************************************************************
c          Area Coverage mode
200   write(22,2) 2.0,xfqe,beammain,-999.,cond,diel
      freq=xfqs
      call ant90_calc(freq,0.,8.,g,aeff,*940)
      write(22,201) freq,aeff
201   format(10x,f7.3,'MHz eff=',f10.3)
      do 250 iazim=0,359
      azimuth=iazim
      do 220 iel=0,90
      elev=iel
220   call ant90_calc(freq,azimuth,elev,gain(iel+1),aeff,*940)
250   write(22,251) iazim,gain
251   format(i5,(T10,10F7.3))
c****************************************************************
500   call ant90_close
      close(22)
c****************************************************************
      go to 999
c****************************************************************
900   write(*,901) run_directory(1:nch_run)//'\anttyp90.dat'
901   format(' In ANTTYP90.EXE, could not OPEN file=',a)
      stop 'OPEN error in ANTTYP90.EXE at 900'
910   write(*,911) filename
911   format(' In ANTTYP90.EXE, error READing file=',a)
      stop 'READ error in ANTTYP90.EXE at 910'
920   write(*,921) run_directory(1:nch_run)//'\anttyp90.dat'
921   format(' In ANTTYP90.EXE, error READing file=',a)
      stop 'READ error in ANTTYP90.EXE at 920'
c***********************************************************************
930   write(*,931)
931   format('ANTTYP90.EXE must be executed:',/
     +  '1. Point-to-Point:',/
     +  '   .....\bin_win\anttyp90.exe run_directory',/,
     +  '4. Area Coverage:',/
     +  '   .....\bin_win\icepacw.exe run_directory a')
      write(*,932)
932   format(/
     +  'Where:',/
     +  '      run_directory = full pathname to RUN directory',/
     +  '                      (e.g. c:\itshfbc\run)')
      stop 'ANTTYP90.EXE not executed properly.'
940   write(*,941) filename
941   format(' In ANTTYP90.EXE, error Calculating from file=',a)
      stop 'READ error in ANTTYP90.EXE at 940'
c***********************************************************************
999   continue
      call destroy_window(window_handle)
      END
c--------------------------------------------------------------
