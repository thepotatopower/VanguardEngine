-- ドラゴニック・オーバーロード・ジ・エンド

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.VC)
	ability1.SetActivation("Cont")
	-- on battle ends
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnBattleEnds)
	ability2.SetLocation(l.VC)
	ability2.SetProperty(p.OncePerTurn)
	ability2.SetTrigger("OnBattleEndsTrigger")
	ability2.SetCost("OnBattleEndsCost")
	ability2.SetCanFullyResolve("OnBattleEndsCanFullyResolve")
	ability2.SetActivation("OnBattleEnds")
end

function Cont()
	if obj.IsPlayerTurn() and obj.Exists({q.Location, l.Soul, q.Name, obj.GetNameFromCardID("dbt02_001")}) then
		obj.AddCardState({q.Other, o.This}, cs.DriveCannotDecrease, p.Continuous)
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 5000, p.Continuous)
	end
end

function OnBattleEndsTrigger()
	return obj.IsAttackingUnit()
end

function OnBattleEndsCost(check)
	if check then return obj.CanCB(1) and obj.CanDiscard(2) end
	obj.CounterBlast(1)
	obj.Discard(2)
end

function OnBattleEndsCanFullyResolve()
	return obj.Exists({q.Location, l.PlayerUnits, q.Other, o.Resting, q.Other, o.ThisFieldID})
end

function OnBattleEnds()
	obj.Stand({q.Other, o.ThisFieldID})
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusDrive, -1, p.UntilEndOfTurn)
end