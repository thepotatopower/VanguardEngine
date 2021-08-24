-- The Hour of Holy Judgement Cometh

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrder, t.Order, p.HasPrompt, p.CB, 2
	end
end

function CheckCondition(n)
	if n == 1 then
		return true
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
		obj.Draw(2)
		obj.ChooseAddTempPower(1, 5000)
	end
	return 0
end