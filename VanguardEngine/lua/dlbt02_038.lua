-- MiMish リカシェンナ

function RegisterAbilities()
	-- friend cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC, l.VC)
	ability1.SetActivation("Cont1")
	-- free ride deck
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.Cont)
	ability2.SetLocation(l.VC)
	ability2.SetActivation("Cont2")
end

function Cont1()
	if obj.GetNumberOf("Cont1Filter", {l.Units}) > 0 then
		obj.AddCardState("MakeFriendFilter", {l.Units}, cs.Friend, p.Continuous)
	end
end

function Cont1Filter(id)
	return not obj.IsThis(id) and obj.IsPlayer(id) and obj.IsSameColumn(id)
end

function MakeFriendFilter(id)
	return obj.IsPlayer(id) and obj.IsSameColumn(id)
end

function Cont2()
	if obj.GetNumberOf("FriendExists", {l.Units}) > 0 then
		obj.AddPlayerString(ps.CanRideFromRideDeckWithoutDiscard, "dlbt02_004", p.Continuous)
	end
end

function FriendExists(id)
	return obj.IsPlayer(id) and not obj.IsThis(id) and obj.HasCardState(id, cs.Friend)
end