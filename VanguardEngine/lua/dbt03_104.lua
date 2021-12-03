-- 星空の魔法 マルルナ

function RegisterAbilities()
	-- on place 
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Cost(check)
	if check then return obj.CanSpecificDiscard({q.Location, l.Hand, q.Name, obj.GetNameFromCardID("dbt01_008")}) end
	obj.SpecificDiscard({q.Location, l.Hand, q.Name, obj.GetNameFromCardID("dbt01_008")})
end

function CanFullyResolve()
	return obj.Exists({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dbt01_008")})
end

function Activation()
	obj.Select({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dbt01_008"), q.Count, 1})
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 10000, p.UntilEndOfTurn)
end