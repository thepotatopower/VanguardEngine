-- 封焔の剣 プリティヴイー

function RegisterAbilities()
	-- arm
	local ability0 = NewAbility(GetID())
	ability0.SetArmTarget(ability0.GetNameFromCardID("dsd06_001"))
	-- on order
	local ability1 = NewAbility(GetID())
	ability1.SetTiming(a.OnOrder)
	ability1.SetCost("OnOrderCost")
	ability1.SetCanFullyResolve("OnOrderCanFullyResolve")
	ability1.SetActivation("OnOrder")
	-- on attack
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(1)
	ability2.SetTiming(a.OnAttack)
	ability2.SetLocation(l.VC)
	ability2.SetProperty(p.OncePerTurn)
	ability2.SetTrigger("OnAttackTrigger")
	ability2.SetCondition("OnAttackCondition")
	ability2.SetActivation("OnAttack")
end

function OnOrderCost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function OnOrderCanFullyResolve()
	return obj.Exists({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dsd06_001")})
end

function OnOrder()
	obj.Arm({q.Other, o.This})
end

function OnAttackTrigger()
	if obj.Exists({q.Location, l.MyArmedUnit, q.Other, o.Attacking}) then
		obj.StoreIDs({q.Location, l.MyArmedUnit, q.Other, o.Attacking})
		return true
	end
	return false
end

function OnAttackCondition()
	return obj.Exists({q.Location, l.Stored, q.Other, o.SameZone})
end

function OnAttack()
	obj.AddCardValue({q.Location, l.Stored, q.Other, o.SameZone}, cs.BonusPower, 10000, p.UntilEndOfBattle)
	local timedTrigger = NewAbility(GetID())
	timedTrigger.SetDescription(1)
	timedTrigger.SetTiming(a.OnBattleEnds)
	timedTrigger.SetCondition("OnBattleEndsCondition")
	timedTrigger.SetCost("OnBattleEndsCost")
	timedTrigger.SetActivation("OnBattleEndsActivation")
	timedTrigger.SetResetTiming(p.UntilEndOfBattle)
end

function OnBattleEndsCondition()
	return obj.GetEnemyDamage() <= 4
end

function OnBattleEndsCost(check)
	if check then return obj.CanCB(2) and obj.CanAddToDrop({q.Other, o.ThisFieldID}) end
	obj.CounterBlast(2)
	obj.AddToDrop({q.Other, o.ThisFieldID})
end

function OnBattleEndsActivation()
	obj.DealDamage(1)
end