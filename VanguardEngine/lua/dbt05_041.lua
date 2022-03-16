-- 救いの泥濘 ケイオス

function RegisterAbilities()
	-- on ride
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnRide)
	ability1.SetTrigger("Trigger")
	ability1.SetActivation("Activation")
	-- glitter
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetProperty(p.IsGlitter)
end

function Trigger()
	return obj.WasRodeUponBy(obj.GetNameFromCardID("dbt05_008"))
end

function Activation()
	obj.Store(obj.SoulCharge(1))
	if obj.Exists({q.Location, l.Stored, q.UnitType, u.Trigger}) then
		obj.Draw(1)
	elseif obj.Exists({q.Location, l.Stored, q.UnitType, u.Normal}) then
		obj.SuperiorCall({q.Location, l.Soul, q.Count, 1})
	end
end