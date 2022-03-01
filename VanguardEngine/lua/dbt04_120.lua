-- 大義の進軍

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnBlitzOrder)
	ability1.CanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function CanFullyResolve()
	return obj.Exists({q.Location, l.PlayerVC, q.Location, l.PlayerRC, q.Name, obj.GetNameFromCardID("dbt04_009")})
end

function Activation()
	obj.Select({q.Location, l.PlayerVC, q.Location, l.PlayerRC, q.Name, obj.GetNameFromCardID("dbt04_009"), q.Count, 1})
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 20000, p.UntilEndOfBattle)
end