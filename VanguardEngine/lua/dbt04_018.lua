-- 封焔の槍 アーディティヤ

function RegisterAbilities()
	-- arm 
	local ability0 = NewAbility(GetID())
	ability0.SetArmTarget(ability0.GetNameFromCardID("dsd06_001"))
	-- order
	local ability1 = NewAbility(GetID())
	ability1.SetTiming(a.OnOrder)
	ability1.SetCost("OnOrderCost")
	ability1.SetActivation("OnOrder")
	-- on armed
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(1)
	ability2.SetTiming(a.OnArmed)
	ability2.SetLocation(l.VC)
	ability2.SetTrigger("OnArmedTrigger")
	ability2.SetActivation("OnArmed")
	-- cont
	local ability3 = NewAbility(GetID())
	ability3.SetDescription(2)
	ability3.SetTiming(a.Cont)
	ability3.SetLocation(l.VC)
	ability3.SetActivation("Cont")
end

function OnOrderCost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function OnOrder()
	obj.Arm({q.Other, o.This})
end

function OnArmedTrigger()
	return obj.WasArmedOnto()
end

function OnArmed()
	obj.Draw(1)
	obj.IncrementUntilEndOfTurnPlayerValue(ps.AdditionalArms, 1)
end

function Cont()
	if obj.IsPlayerTurn() then
		obj.AddCardValue({q.Location, l.MyArmedUnit}, cs.BonusPower, 10000, p.Continuous)
	end
end