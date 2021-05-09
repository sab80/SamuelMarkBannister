# -*- coding: utf-8 -*-
"""
Created on Thu Apr 15 02:38:50 2021

@author: SMBtr
"""

from distutils.core import setup
import py2exe

setup(windows=['MainWindow.py'] ,options = {"py2exe":{"includes":["PyQt5.Qt","PyQt5.sip","PyQt5.QtWidgets","PyQt5.QtCore","PyQt5.QtGui"]}}) 
      
      
      