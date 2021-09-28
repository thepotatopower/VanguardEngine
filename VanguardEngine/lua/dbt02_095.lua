-- Wienlens Dragon

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.NameContains, "Blaster"
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnRetiredForPlayerCost, p.HasPrompt, p.CB, 1, p.SB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.WasRetiredForPlayerCost() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.GetNumberOf(1) > 0 then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.AddTempPower(1, 5000)
	end
	return 0
end