-- ディアブロスボーイズ ナイル

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.GC)
	ability1.SetActivation("Cont")
end

function Cont()
	if obj.GetNumberOf("Filter", {l.MyUnitsHit}) > 0 then
		obj.AddCardValue({q.Other, o.This}, cs.BonusShield, 15000, p.Continuous)
	end
end

function Filter(snapshot)
	return obj.NameContains(snapshot.name, obj.LoadName("Bruce"))
end