-- Steam Detective, Urvaritt

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.Column, 0, q.Other, o.NotThis, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRC, p.HasPrompt, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnRC() then
			obj.Inject(1, q.Column, obj.GetColumn())
			if obj.Exists(1) then
				return true
			end
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
		obj.Inject(1, q.Column, obj.GetColumn())
		if obj.InFinalRush() then
			obj.ChooseAddTempPower(1, 5000)
		else
			obj.ChooseAddTempPower(1, 2000)
		end
	end
	return 0
end