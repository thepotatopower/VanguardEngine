-- 蒼砲竜 インレットパルス・ドラゴン

function RegisterAbilities()
	-- on battle ends
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnBattleEnds)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("OnBattleEndsTrigger")
	ability1.SetCondition("OnBattleEndsCondition")
	ability1.SetCost("OnBattleEndsCost")
	ability1.SetCanFullyResolve("OnBattleEndsCanFullyResolve")
	ability1.SetActivation("OnBattleEnds")
	-- on end phase
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnEndPhase)
	ability2.SetLocation(l.RC)
	ability2.SetTrigger("OnEndPhaseTrigger")
	ability2.SetCondition("OnEndPhaseCondition")
	ability2.SetCost("OnEndPhaseCost")
	ability2.SetActivation("OnEndPhase")
end

function OnBattleEndsTrigger()
	return obj.IsAttackingUnit() and obj.Exists({q.Location, l.EnemyVC, q.Grade, 3, q.Other, o.GradeOrHigher, q.Other, o.Attacked})
end

function OnBattleEndsCondition()
	return obj.NumOfAttacks() == 1 and obj.Exists({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dbt03_009")})
end

function OnBattleEndsCost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function OnBattleEndsCanFullyResolve()
	return obj.Exists({q.Other, o.ThisFieldID, q.Other, o.Resting})
end

function OnBattleEnds()
	obj.Stand({q.Other, o.ThisFieldID})
end

function OnEndPhaseTrigger()
	return obj.IsPlayerTurn()
end

function OnEndPhaseCondition()
	return obj.NumOfAttacks() >= 4
end

function OnEndPhaseCost(check)
	if check then return obj.CanAddToSoul({q.Other, o.ThisFieldID}) end
	obj.AddToSoul({q.Other, o.ThisFieldID})
end

function OnEndPhase()
	obj.Draw(1)
end