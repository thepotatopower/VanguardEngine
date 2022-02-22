-- ディアブロス “暴虐” ブルース

function RegisterAbilities()
	-- on ride phase
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnRidePhase)
	ability1.SetLocation(l.VC)
	ability1.SetTrigger("OnRidePhaseTrigger")
	ability1.SetActivation("OnRidePhase")
	-- on attack
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnAttack)
	ability2.SetLocation(l.VC)
	ability2.SetTrigger("OnAttackTrigger")
	ability2.SetCondition("OnAttackCondition")
	ability2.SetCost("OnAttackCost")
	ability2.SetCanFullyResolve("OnAttackCanFullyResolve")
	ability2.SetActivation("OnAttack")
end

function OnRidePhaseTrigger()
	return obj.IsPlayerTurn()
end

function OnRidePhase()
	obj.FinalRush()
end

function OnAttackTrigger()
	return obj.IsAttackingUnit()
end

function OnAttackCondition()
	return obj.InFinalRush()
end

function OnAttackCost(check)
	if check then return obj.CanSB(5) end
	obj.SoulBlast(5)
end

function OnAttackCanFullyResolve()
	return obj.Exists({q.Location, l.FrontRowRC, q.Other, o.Resting})
end

function OnAttack()
	obj.Stand({q.Location, l.FrontRowRC})
end
