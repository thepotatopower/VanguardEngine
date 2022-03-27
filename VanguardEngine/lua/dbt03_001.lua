-- ヴェルリーナ・エクスペクター

function RegisterAbilities()
	-- overDress requirement
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetOverDress("IsOverDressTarget")
	-- continuous ability
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.Cont)
	ability2.SetLocation(l.PlayerRC)
	ability2.SetActivation("Continuous")
	-- on attack
	local ability3 = NewAbility(GetID())
	ability3.SetDescription(3)
	ability3.SetTiming(a.OnAttack)
	ability3.SetTriggerCondition("OnAttackTrigger")
	ability3.SetLocation(l.PlayerRC)
	ability3.SetCost("Cost")
	ability3.SetActivationCondition("OnAttackCondition")
	ability3.SetActivation("OnAttack")
end

function IsOverDressTarget(id)
	return obj.IsRearguard(id) and obj.IsOverDress(id) 
end

function Continuous()
	obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.This}, cs.BonusPower, obj.GetTotalOriginalPower({q.Location, l.MyOriginalDress}), p.Continuous)
end

function OnAttackTrigger()
	if obj.IsAttackingUnit() and obj.Exists({q.Location, l.EnemyVC, q.Grade, 3, q.Other, o.GradeOrHigher, q.Other, o.Attacked}) then
		obj.Track(GetID())
		return true
	else
		return false
	end
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function OnAttackCondition()
	return obj.IsSameZone(GetID()) and obj.IsOverDress(GetID()) 
end

function OnAttack()
	obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.This}, cs.BonusPower, 15000, p.UntilEndOfBattle)
	obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.This}, cs.BonusCritical, 1, p.UntilEndOfBattle)
	local timedTrigger = NewAbility(GetID()) 
	timedTrigger.SetTiming(a.OnBattleEnds)
	timedTrigger.SetActivation("OnEndOfBattle")
	timedTrigger.SetResetTarget(GetID())
	timedTrigger.SetResetTiming(p.UntilEndOfBattle)
end

function OnEndOfBattle()
	obj.AddToDrop({q.Location, l.MyOriginalDress})
end