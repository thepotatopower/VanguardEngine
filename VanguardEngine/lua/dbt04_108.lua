-- 奈落竜の冥加

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnBlitzOrder)
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function CanFullyResolve()
	return obj.Exists({q.Location, l.PlayerVC, q.NameContains, obj.LoadName("Blaster")})
end

function Activation()
	obj.Select({q.Location, l.PlayerVC, q.NameContains, obj.LoadName("Blaster"), q.Count, 1})
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 15000, p.UntilEndOfBattle)
	obj.ChooseRetire({q.Location, l.PlayerRC, q.Count, 1, q.Min, 0})
	if obj.GetNumberOf({q.Location, l.SuccessfullyRetired}) > 0 then
		obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 15000, p.UntilEndOfBattle)
	end
end