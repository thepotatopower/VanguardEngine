-- 封焔の銃 チャンドラ

function RegisterAbilities()
	-- arm 
	local ability0 = NewAbility(GetID())
	ability0.SetArmTarget(ability0.GetNameFromCardID("dsd06_001"))
	-- order
	local ability1 = NewAbility(GetID())
	ability1.SetTiming(a.OnOrder)
	ability1.SetCost("OnOrderCost")
	ability1.SetActivation("OnOrder")
	-- on attack
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(1)
	ability2.SetTiming(a.OnAttack)
	ability2.SetLocation(l.VC)
	ability2.SetTrigger("OnAttackTrigger")
	ability2.SetCost("OnAttackCost")
	ability2.SetActivation("OnAttack")
end

function OnOrderCost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function OnOrder()
	obj.Arm({q.Other, o.This})
end

function OnAttackTrigger()
	return obj.Exists({q.Location, l.MyArmedUnit, q.Other, o.Attacking})
end

function OnAttackCost(check)
	if check then return obj.CanCB(2) end
	obj.CounterBlast(2)
end

function OnAttack()
	obj.AddCardValue({q.Location, l.MyArmedUnit, q.Other, o.SameZone}, cs.BonusDrive, 1, p.UntilEndOfBattle)
	local timedTrigger = GiveAbility(GetID(), GetID())
	timedTrigger.SetTiming(a.OnBattleEnds)
	timedTrigger.SetActivation("TimedTrigger")
	timedTrigger.SetResetTiming(p.UntilEndOfBattle)
	timedTrigger.SetResetTarget(GetID())
end

function TimedTrigger()
	obj.AddToDrop({q.Other, o.ThisFieldID})
end