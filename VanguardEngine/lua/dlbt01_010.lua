-- Earnescorrect Member, Evelyn

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Deck, q.NameContains, "Earnescorrect", q.Grade, 2, q.Other, o.GradeOrLess, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Revealed, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Name, "", q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRCFromHand, p.HasPrompt, p.CB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnRCFromHand() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n) 
	if n == 1 then
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.ChooseReveal(1)
		obj.Inject(3, q.Name, obj.GetName(2))
		if not obj.Exists(3) and obj.CanSuperiorCall(2, FL.OpenCircle) then
			obj.SuperiorCall(2, FL.OpenCircle)
		end
		obj.Shuffle()
	end
	return 0
end