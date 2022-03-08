-- やる気充分！ アルレット

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
	ability2.SetLocation(l.RC)
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
	return obj.GetNumberOf("OnBattleEndsFilter", {l.Units}) >= 3
end

function OnBattleEndsFilter(id)
	return obj.IsPlayer(id) and not obj.IsThis(id) and obj.HasCardState(id, cs.Friend)
end

function OnBattleEndsCost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function OnBattleEndsCanFullyResolve()
	return obj.IsSameZone()
end

function OnBattleEnds()
	obj.AddToHand({q.Other, o.ThisFieldID})
end