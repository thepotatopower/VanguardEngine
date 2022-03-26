-- インディケイドアロー・ドラゴン

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("Trigger")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Activation()
	obj.RevealFromDeck(1)
	if obj.GetNumberOf("Filter", {l.Soul}) < 1 then
		obj.SoulCharge(1)
	end
end

function Filter(id)
	local name = obj.GetName({q.Location, l.Revealed})
	return obj.NameIs(id, name)
end