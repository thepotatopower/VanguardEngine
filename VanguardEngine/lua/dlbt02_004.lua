-- MiMish フォルティア

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetProperty(p.Friend)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC, l.VC)
	ability1.SetActivation("Cont")
	-- on end of battle
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnBattleEnds)
	ability2.SetLocation(l.VC)
	ability2.SetProperty(p.OncePerTurn)
	ability2.SetTrigger("OnBattleEndsTrigger")
	ability2.SetCondition("OnBattleEndsCondition")
	ability2.SetCost("OnBattleEndsCost")
	ability2.SetCanFullyResolve("OnBattleEndsCanFullyResolve")
	ability2.SetActivation("OnBattleEnds")
end

function Cont()
	if obj.GetNumberOf("ContFilter", {l.Units}) > 0 then
		obj.AddCardState("FriendFilter", {l.Units}, cs.Friend, p.Continuous)
	end
end

function ContFilter(id)
	return not obj.IsThis(id) and obj.IsPlayer(id) and obj.IsSameColumn(id)
end

function FriendFilter(id)
	return obj.IsPlayer(id) and obj.IsSameColumn(id)
end

function OnBattleEndsTrigger()
	return obj.IsAttackingUnit()
end

function OnBattleEndsCondition()
	return obj.GetNumberOf("OnBattleEndsConditionFilter", {l.Units}) >= 5 
end

function OnBattleEndsConditionFilter(id)
	return obj.IsPlayer(id) and not obj.IsThis(id) and obj.HasCardState(id, cs.Friend)
end

function OnBattleEndsCost(check)
	if check then return obj.CanCB(1) and obj.CanDiscard(1) end
	obj.CounterBlast(1)
	obj.Discard(1) 
end

function OnBattleEndsCanFullyResolve()
	return obj.IsSameZone()
end

function OnBattleEnds()
	obj.Stand("OnBattleEndsFilter", {l.Units})
	obj.AddCardValue("OnBattleEndsFilter", {l.Units}, cs.BonusPower, 5000, p.UntilEndOfTurn)
	obj.AddCardValue("OnBattleEndsFilter", {l.Units}, cs.BonusDrive, -1, p.UntilEndOfTurn)
end

function OnBattleEndsFilter(id)
	return obj.IsThis(id) and obj.IsSameZone(id)
end